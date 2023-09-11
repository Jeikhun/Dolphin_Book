let plusBtn = document.querySelectorAll(".bi-plus-lg");
let minusBtn = document.querySelectorAll(".bi-dash-lg");

plusBtn.forEach(element => {
    element.addEventListener("click", (e)=>{
        // if(e.target.parentElement.parentElement.parentElement.lastElementChild.style.display=="block" ){

        //     console.log(e.target.parentElement.parentElement.parentElement.lastElementChild);
        //     e.target.parentElement.parentElement.parentElement.lastElementChild.style.display="none";
        //     e.target.parentElement.parentElement.firstElementChild.style.color = "black"
            
        // }
    if(e.target.parentElement.parentElement.parentElement.lastElementChild.style.display=="none"||e.target.parentElement.parentElement.parentElement.lastElementChild.style.display==""){
        console.log("Aaaa");
        e.target.parentElement.parentElement.parentElement.lastElementChild.style.display="block";
        e.target.parentElement.parentElement.firstElementChild.style.color = "rgb(12, 69, 245)"
        e.target.parentElement.lastElementChild.style.display = "initial"
            e.target.style.display = "none"
    }
    });
});


minusBtn.forEach(element => {
    element.addEventListener("click", (e)=>{
        if(e.target.parentElement.parentElement.parentElement.lastElementChild.style.display=="block" ){

            console.log(e.target.parentElement.parentElement.parentElement.lastElementChild);
            e.target.parentElement.parentElement.parentElement.lastElementChild.style.display="none";
            e.target.parentElement.parentElement.firstElementChild.style.color = "black"
            e.target.parentElement.firstElementChild.style.display = "initial"
            e.target.style.display = "none"
        }

    });
});


