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

$(".category").click(function () {
    var dataid = $(this).attr("data-id")
    $(".product").addClass("d-none")
    $(`.product[data-target='${dataid}']`).removeClass("d-none")

})

$(".category").eq(0).click();


$(function () {
    $(document).scroll(function () {
        var $nav = $(".navbar");
        $nav.toggleClass('bg-white', $(this).scrollTop() > $nav.height());
    });
});