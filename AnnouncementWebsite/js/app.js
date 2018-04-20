var app = angular.module("moneySaver", []);
app.factory("transcCategory", function () {
    var category = {};
    category.expenses = ["Food and Beverages", "Bill Payment", "Entertainment", "Shopping", "Transportation", "Travel", "Health and Fitness", "Education", "Other"];
    
    category.income = ["Salary", "Award", "Pocket Money", "Other"];
    return category;
});

app.controller("transacCtrl", function ($scope) {
    "use strict";
    $scope.transactions = ["s"];
});

app.controller("addTranscCtrl", function ($scope, transcCategory) {
    $scope.transc = {};
    $scope.transc.date = new Date();
    $scope.expensesCtgs = transcCategory.expenses;
    $scope.incomeCtgs = transcCategory.income;
    
    $scope.checkIsNum = function (value) {
        if (angular.isNumber(value)) {
            return true;    
        } else {
            return false;
        }
    };
});

app.controller("productListCtrl", function($scope){
    $scope.products = [{id:"1",name:"testA"}, {id:"2",name:"test B"}, {id:"3",name:"test C"}, {id:"4",name:"test D"}, {id:"5",name:"test E"}];
});

app.controller("shoppingBasketCtrl", function ($scope) {

});