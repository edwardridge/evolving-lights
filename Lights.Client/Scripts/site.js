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
    var that = this;
    that.totalDifferenceInFitness = 0;
    that.chaningColor = false;
    that.testNgStyle = {"background-color": "rgb(0, 100, 0)"}
    this.testChangeColor = function (){
        that.testNgStyle = {"background-color": "rgb(200, 100, 100)"}
    } 
    
    this.changeColors = false;
    
    this.populationsToGenerate = 500;
    
    this.generateInitialPopulation = function () {
        $http.get(window.apiBase + "GetInitialPopulation").then(function(result){
            that.colorLines = result.data.map(that.mapResult);
        });
    }
    
    this.mapResult = function (result) {
        return {
            "Colors": result.Colors.map(color => {
                return that.setCssColor(color);
            }),
            "Fitness": result.Fitness
        };
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
            var newColors = result.data.map(that.mapResult);
            
            
            for (var i = 0; i < that.colorLines.length; i++){
                for (var j = 0; j < that.colorLines[i].Colors.length; j++){
                    that.colorLines[i].Colors[j]["background-color"] = newColors[i].Colors[j]["background-color"];
                }
            }
            
            that.totalDifferenceInFitness = Math.max(...newColors.map(s => -s.Fitness));
            if (that.totalDifferenceInFitness < 5){
                if (that.chaningColor === false){
                    that.chaningColor = true;
                    $timeout(() => {

                        var red = Math.floor(Math.random() * 256);
                    var green = Math.floor(Math.random() * 256);
                    var blue = Math.floor(Math.random() * 256);
                    var data = {
                        "red": red,
                        "green": green,
                        "blue": blue
                    }
                    $.post(window.apiBase + "SetTargetColor", data).then(function(result){
                        that.chaningColor = false;
                        $('body,html').css('background-color','rgb(' + red + ', ' + green + ', ' + blue + ')');
                    }).catch(function (error){
                        console.log(error.data);
                    });
                }, 1000);
                }
                
                
            }
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