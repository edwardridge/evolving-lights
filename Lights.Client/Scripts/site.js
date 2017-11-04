window.apiBase = "http://127.0.0.1:5001/Lights/";

$(function() {
    $.get(window.apiBase + "GetTargetColor").then(function (result)   {
        var test = rgbToHex(result.Red, result.Green, result.Blue);
        var $cp2 = $('#cp2');
        $('body,html').css('background-color','rgb(' + result.Red + ', ' + result.Green + ', ' + result.Blue+ ')');
        $cp2.colorpicker({
            format: 'rgb',
            color: test
        }).on('hidePicker', function(e) {
            var rgb = e.color.toRGB();
            var data = {
                "red": rgb.r,
                "green": rgb.g,
                "blue": rgb.b
            }
            $.post(window.apiBase + "SetTargetColor", data).then(function(result){
                $('body,html').css('background-color','rgb(' + rgb.r + ', ' + rgb.g + ', ' + rgb.b + ')');
            }).catch(function (error){
                console.log(error.data);
            });
        });
    });
    
});

function componentToHex(c) {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}

function rgbToHex(r, g, b) {
    return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
}

// Write your Javascript code.
angular.module('MyApp', [])
    .controller('lights', ['$timeout', '$http', lights])

function lights($timeout, $http){
    this.changeColors = false;
    
    var that = this;
    this.populationsToGenerate = 500;
    
    this.generateInitialPopulation = function () {
        $http.get(window.apiBase + "GetInitialPopulation").then(function(result){
            that.colorLines = result.data;
        });
    }
    
    this.generateInitialPopulation();
    $http.get(window.apiBase + "GetEvolutionDetails").then(function(result){
        that.evolutionDetails = result.data;
    });
    
    this.setCssColor = function(color){
        return {
            "background-color":
            "rgb(" + color.Red + ", " + color.Green + ", " + color.Blue + ")"
        }
    }

    this.getNextPopulations = function(populations) {
        var data = {"populations": populations};
        $http.post(window.apiBase + "GetNextPopulation", data).then(function (result) {
            that.colorLines = result.data;
        });
    }

    this.repeatingNextPopulation = function () {
        if (that.changeColors){
            that.getNextPopulations(that.populationsToGenerate);
        }
        
        $timeout(function(){
            that.repeatingNextPopulation();
        }, 500);
    }

    this.repeatingNextPopulation();
}