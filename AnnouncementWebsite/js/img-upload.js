var app = angular.module("moneySaver", []);
app.controller("addProductCtrl", function($scope) {
    //For image upload
    $scope.test = "hi";
    $scope.images = [];
    $scope.imgName = "Click 'Browse' to add photo, multiple photos may be added";
	$scope.isShpgAgentSelected = false;
    
    //For preventing form submission when inputs are invalid
    $scope.submitted = false;
    
    //For image upload
    $scope.getImgUrl = function(event) {
        var files = event.target.files;
    	if ($scope.imgName == "Click 'Browse' to add photo, multiple photos may be added") {
			$scope.imgName = "";
		}
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
			
			if ($scope.imgName == "") {
				$scope.imgName = file.name;
			} else {
				$scope.imgName += "," + file.name;
			}
            
            var fileReader = new FileReader();
            fileReader.onload = $scope.loadImg;
            fileReader.readAsDataURL(file);
        }
   };
    
    $scope.loadImg = function(e){
        $scope.$apply(function(){
           $scope.images.push(e.target.result); 
        });
    };
	
	/*$scope.checkAgents = function() {
		$scope.isShpgAgentSelected = false;
		for(var i = 0; i < 6; i++) {
			if ($scope.shpgAgent[i] == true) {
				$scope.isShpgAgentSelected = true;
			}
		}
		return $scope.isShpgAgentSelected;
	};*/
    
    //For preventing form submission when inputs are invalid
    $scope.productSubmit = function(e){   
        if($scope.frmProduct.$invalid){
            e.preventDefault();
            $scope.submitted = true;
            $scope.test = "no submit";
        }
    };
    
});