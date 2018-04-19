function imgModal(){
    
    outputimagemodal("item-display");
}


function subimgModal(subimageid){
    
    if(subimageid == "item-1"){
        outputimagemodal("item-1");
    }else if(subimageid == "item-2"){
        outputimagemodal("item-2");
    }else if(subimageid == "item-3"){
        outputimagemodal("item-3");
    }
    
}

function outputimagemodal(id){
    var img = document.getElementById(id);
    var modal = document.getElementById('myModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");

    modal.style.display = "block";
    modalImg.src = img.src;
    captionText.innerHTML = img.alt;
}

// When the user click the modal image, it will close
function closemodal(){
    var modal = document.getElementById('myModal');
    modal.style.display = "none";
} 