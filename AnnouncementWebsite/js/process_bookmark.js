function manageBookmark(jsonData) {
    var bookmarkImgUrl = document.getElementById(jsonData.bookmarkId).src;
    var bookmarkImgArr = bookmarkImgUrl.split("/");
    var bookmarkImg = bookmarkImgArr[bookmarkImgArr.length-1];
    if (jsonData.user != null && jsonData.user != "") {
        if (bookmarkImg == "bookmark.png") {
            addBookmark(jsonData);
        } else {
            removeBookmark(jsonData);
        }
    } else {
        alert("Please login to bookmark the tender.");   
    }
}

function addBookmark(jsonData) {
    //Send request to bookmark the selected tender
    //alert("Hello"); 
    
    console.log(jsonData);
    console.log(jsonData.tenderRef);
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            var response = this.responseText;
            console.log("Response: " + this.responseText);
            if (response == "Success") {
                alert("Tender has been successfully bookmark!");
                document.getElementById(jsonData.bookmarkId).src = "../images/bookmarkfilled.png";
            } else {
                alert("Failed to bookmark tender. Please try again!");
            }
        }
    };

    xmlHttp.open("POST", "../process_manageTenderBookmark.php", true);
    xmlHttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    var data = "username=" + jsonData.user + "&tenderReferenceNumber=" + encodeURIComponent(jsonData.tenderRef) + "&tenderTitle=" + encodeURIComponent(jsonData.tenderTitle) + "&originatingSource=" + jsonData.originatingSource + "&closingDate=" + jsonData.closingDate;
    console.log(data);
    xmlHttp.send(data);   
}

function confirmRemoveBookmark(jsonData) {
    var confirmDelete = confirm("Are you sure you want to delete this bookmark?");
    if (confirmDelete) {
        removeBookmark(jsonData);
        //location.reload(true);
    }
}

function removeBookmark(jsonData) {
    //Send request to bookmark the selected tender
    //alert("Hello"); 
    //alert("Delete");
    console.log(jsonData);
    
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            var response = this.responseText;
            console.log("Delete Response: " + this.responseText);
            if (response == "Success") {
                alert("Bookmark has been successfully removed!");
                if (jsonData.bookmarkId.includes("bookmarkTender")) {
                    var divElement = document.getElementById(jsonData.bookmarkId);
                    divElement.parentNode.removeChild(divElement);
                } else {
                    document.getElementById(jsonData.bookmarkId).src = "../images/bookmark.png";
                }
            } else {
                alert("Failed to remove bookmark. Please try again!");
            }
        }
    };

    xmlHttp.open("POST", "../process_manageTenderBookmark.php", true);
    xmlHttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    var data = "username=" + jsonData.user + "&tenderReferenceNumber=" + jsonData.tenderRef + "&tenderTitle=" + jsonData.tenderTitle + "&isDelete=true";
    xmlHttp.send(data);
    return 0;
}