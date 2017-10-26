// Write your Javascript code.
angular.module('MyApp', [])
    .controller('lights', ['$timeout', '$http', lights])

function lights($timeout, $http){
    // this.colorLines = createColors();
    this.changeColors = false;
    var that = this;

    $http.get("http://127.0.0.1:5001/Home/GetInitialPopulation").then(function(result){
        that.colorLines = result.data.map(s => s.Colors);;
    });

    $http.get("http://127.0.0.1:5001/Home/GetEvolutionDetails").then(function(result){
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
            $http.get("http://127.0.0.1:5001/Home/GetNextPopulation").then(function(result){
                that.colorLines = result.data.map(s => s.Colors);
            });
        }
        
        $timeout(function(){
            that.nextPopulation();
        }, 500);
    }

    this.nextPopulation();
}