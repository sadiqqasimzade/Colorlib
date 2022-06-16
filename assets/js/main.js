var menubtn=document.getElementById("menubtn");
var aside=document.getElementById("aside");

menubtn.addEventListener('click',function(){
    if(aside.classList.contains("asidedeactive"))
    {
        aside.classList.add("asideactive")
        aside.classList.remove("asidedeactive")
    }
    else{
        aside.classList.add("asidedeactive")
        aside.classList.remove("asideactive")
    }

})