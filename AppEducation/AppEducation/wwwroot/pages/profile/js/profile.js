var newinput = document.getElementsByClassName("newInput");
var oldinput = document.getElementsByClassName("oldInput");
var saveButton = document.getElementById("save");
var changeButton = document.getElementById("changes");
function changeFunction(){
    for (var i = 0; i < newinput.length; i++) {
        newinput[i].style.display = "block";
        oldinput[i].style.display = "none";
    }
    changeButton.onclick = () => changeButton.style.display = "none";
    saveButton.onclick  = () => saveButton.style.display = "block";
}
   
    var modal = document.getElementById("myModal");
    var img = document.getElementById("myImg");
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    
    img.onclick = function(){
        modal.style.display = "block";
        modalImg.src = this.src;
        captionText.innerHTML = this.alt;
    }
    var span = document.getElementsByClassName("close")[0];
    span.onclick = function() { 
        modal.style.display = "none";
    }
