window.onload = function () {
    const kargoPrice = localStorage.getItem("kargo")
    const container = document.querySelector(".free")
    
    if (kargoPrice == 0 || kargoPrice == null) {

        let view = `<span>Pulsuz<span>`
        container += view;
    } else {
        let view = 
            container.innerHTML = `<p>${kargoPrice} </p><span>₼</span>`
    }
}
let checkedCount = 0;
const checkBoxes = document.querySelectorAll("input[name=condition]");
const finishBtn = document.querySelector("#myForm");
const finishButton = document.querySelector("#finish-button");
checkBoxes.forEach(element => {
    element.addEventListener("change", (e) => {
        if (e.target.checked) {
            checkedCount++;
        }
        else {
            checkedCount--;
        }

        if (checkedCount == 2) {
            finishBtn.style.display = "initial";
            finishButton.style.display = "none";
            
        }
        else {
            finishBtn.style.display = "none";
            finishButton.style.display = "initial";
        }
        console.log(checkedCount);

    })

})

