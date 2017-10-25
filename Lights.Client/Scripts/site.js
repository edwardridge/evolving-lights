// Write your Javascript code.
angular.module('MyApp', [])
    .controller('lights', ['$timeout', '$http', lights])

function lights($timeout, $http){
    // this.colorLines = createColors();
    this.changeColors = false;
    var that = this;
    $http.get("http://127.0.0.1:5001/Home/GetInitialPopulation").then(function(result){
        // that.test = r
        that.colorLines = result.data.map(s => s.Colors);;
    });
    
    this.setCssColor = function(color){
        return {
            "background-color":
            "rgb(" + color.Red + ", " + color.Green + ", " + color.Blue + ")"
        }
    }
    
    this.nextPopulation = function () {
        $http.get("http://127.0.0.1:5001/Home/GetNextPopulation").then(function(result){
            that.colorLines = result.data.map(s => s.Colors);
        });

        $timeout(function(){
            that.nextPopulation();
        }, 500);
    }

    // var resetColors = () => {
    //     if(this.changeColors){
    //         this.colorLines = createColors();
    //     }
    //
    //     $timeout(function(){
    //         resetColors();
    //     }, 1000);
    // }
    //
    // resetColors();

    // function createColors(){
    //     const colorLines = [];
    //
    //     for(var numColorLines = 0; numColorLines < 20; numColorLines++){
    //         let colors = [];
    //         for(var numColors = 0; numColors < 20; numColors++){
    //             const red = Math.floor((Math.random() * 255) + 1);
    //             const green = Math.floor((Math.random() * 255) + 1);
    //             const blue = Math.floor((Math.random() * 255) + 1);
    //             colors.push(
    //                 {
    //                     "Red": red,
    //                     "Green": green,
    //                     "Blue": blue
    //                 }
    //             );
    //         }
    //         colorLines.push(colors);
    //     }
    //
    //     return colorLines;
    // }

}