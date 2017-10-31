window.apiBase = "http://127.0.0.1:5001/Lights/";

$(function() {
    var $cp2 = $('#cp2');
    $cp2.colorpicker({
        format: 'rgb'
    }).on('hidePicker', function(e) {
        var rgb = e.color.toRGB();
        var data = {
            "red": rgb.r,
            "green": rgb.g,
            "blue": rgb.b
        }
        $.post(window.apiBase + "SetTargetColor", data).then(function(result){
        }).catch(function (error){
            console.log(error.data);
        });
    });
});

// Write your Javascript code.
angular.module('MyApp', [])
    .controller('lights', ['$timeout', '$http', lights])

function lights($timeout, $http){
    this.changeColors = false;
    
    var that = this;

    $http.get(window.apiBase + "GetInitialPopulation").then(function(result){
        that.colorLines = result.data.map(s => s.Colors);;
    });

    $http.get(window.apiBase + "GetEvolutionDetails").then(function(result){
        that.evolutionDetails = result.data;
    });
    
    this.setCssColor = function(color){
        return {
            "background-color":
            "rgb(" + color.Red + ", " + color.Green + ", " + color.Blue + ")"
        }
    }

    this.nextPopulation = function () {
        if (that.changeColors){
            $http.post(window.apiBase + "GetNextPopulation").then(function(result){
                that.colorLines = result.data.map(s => s.Colors);
            });
        }
        
        $timeout(function(){
            that.nextPopulation();
        }, 500);
    }

    this.nextPopulation();
}