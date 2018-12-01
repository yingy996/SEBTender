function filterTenders() {
    
    //Get all the ticked tender source checkbox and save in array
    var selectedSources = new Array();
    var ulElem = document.getElementById("filterUl");
    var allCheckbox = document.getElementById("allChkb");
    
    if (allCheckbox.checked) {
        window.location.href = "index.php";
    } else {
        var isAllChecked = true;
        
        for (var i = 0; i < ulElem.childNodes.length; i++) {
            if (ulElem.childNodes[i].nodeName == "LI") {
                var liParentNode = ulElem.childNodes[i];
                if (liParentNode.childNodes.length > 0) {
                    var inputElem = liParentNode.childNodes[0].childNodes[0].childNodes[0];
                    if (inputElem.checked == false && inputElem.id != "allChkb") {
                        //Check all the tender source checkbox
                        isAllChecked = false;
                    }
                    
                    //Save source into array if checkbox is checked
                    if (inputElem.checked) {
                        selectedSources[selectedSources.length] = inputElem.value;
                    }
                }
            }
        }
        
        
        if (isAllChecked) {
            window.location.href = "index.php";
        } else {
            //Convert tender source array to JSON
            var filterInputJson = JSON.stringify(selectedSources);
            document.getElementById("filterInput").value = filterInputJson;
            document.getElementById("filterForm").submit();
        }
    }
    
    //console.log(selectedSources[0]);
}

function sortTenders() {
    //Get the selected sort order, sort field and set it as the value of form input
    var orderRadios = document.getElementsByName("sortOrder");
    var selectedSortField = document.getElementById("sortFieldInput").value;
    var selectedOrder = "";
    for (var i = 0; i < orderRadios.length; i++) {
        if (orderRadios[i].checked) {
            selectedOrder = orderRadios[i].value;
            break;
        }
    }
    
    //Set the value of sort order form input
    document.getElementById("sortOrder").value = selectedOrder;
    
    if (selectedSortField == "") {
        //Display error message if no option is selected
        alert("Please select a sort option");
    } else {
        //Get filter data and set the filter form input
        var filterListJson = document.getElementById("jsonStr").textContent;
        if (filterListJson != "") {
            document.getElementById("currentFilter").value = filterListJson;
        }
        
        //Reload page to display tenders in sorted order
        document.getElementById("sortForm").submit();
    }
}

function handleSortingSelect(liElem) {
    //Get the text of the selected sort
    var aElem = liElem.childNodes[0];
    var selectedSort = aElem.text;
    
    //Set the text of the sort dropdown to include selected sort
    var sortBtn = document.getElementById("sortBtn");
    sortBtn.textContent = "Sort by: " + selectedSort;
    
    //Set the value of hidden form input for sort field
    document.getElementById("sortFieldInput").value = selectedSort;
}

//Check for filter checkbox 
function checkFilter(checkboxClicked){
    if (checkboxClicked == "all") {
        //Get the checked value for "ALL" checkbox
        var allCheckbox = document.getElementById("allChkb");
        var isChecked = allCheckbox.checked;
        var ulParentNode = allCheckbox.parentNode.parentNode.parentNode.parentNode;
        //var ilParentNode = allCheckbox.parentNode.parentNode.parentNode;
        //console.log(liParentNode.childNodes[0].childNodes[0].childNodes[0].nodeName);
        //loop through every 'li' node to set the checkbox to checked
        for (var i = 0; i < ulParentNode.childNodes.length; i++) {
            //Get the input element
            if (ulParentNode.childNodes[i].nodeName == "LI") {
                var liParentNode = ulParentNode.childNodes[i];
                if (liParentNode.childNodes.length > 0) {
                    var inputElem = liParentNode.childNodes[0].childNodes[0].childNodes[0];
                    if (inputElem.value != "all") {
                        if (isChecked) {
                            //Check all the tender source checkbox
                            inputElem.checked = true;
                        } else {
                            inputElem.checked = false;
                        }

                    }
                }
            }
        }
    } else {
        var checkboxElem = document.getElementById(checkboxClicked);
        var isChecked = checkboxElem.checked;
        
        var allCheckbox = document.getElementById("allChkb");
        if (!isChecked) {
            //If one tender source checkbox is unchecked, uncheck the "All" checkbox
            allCheckbox.checked = false;
        } else {
            //Check if all tender source checkbox is checked, if all are checked, check the "All" checkbox
            var ulParentNode = allCheckbox.parentNode.parentNode.parentNode.parentNode;
            var isAllChecked = true;
            
            for (var i = 0; i < ulParentNode.childNodes.length; i++) {
                //Get the input element
                if (ulParentNode.childNodes[i].nodeName == "LI") {
                    var liParentNode = ulParentNode.childNodes[i];
                    if (liParentNode.childNodes.length > 0) {
                        var inputElem = liParentNode.childNodes[0].childNodes[0].childNodes[0];
                        if (inputElem.checked == false && inputElem.id != "allChkb") {
                            //Check all the tender source checkbox
                            isAllChecked = false;
                        }
                    }
                }
            }
            
            if (isAllChecked) {
                allCheckbox.checked = true;
            }
        }
    }
}