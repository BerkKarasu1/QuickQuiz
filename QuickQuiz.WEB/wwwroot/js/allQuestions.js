let question = button => {
    let element = document.querySelectorAll(`#Amod_${button},#Bmod_${button},#Cmod_${button},#Dmod_${button},#Emod_${button}`);
    let element2 = document.querySelectorAll(`#AAmod_${button},#BBmod_${button},#CCmod_${button},#DDmod_${button},#EEmod_${button}`);
    let hidden = element[0].getAttribute("hidden");
    if (hidden) {
        for (let i = 0; i < element.length; i++) {
            element[i].removeAttribute("hidden");
            element2[i].removeAttribute("hidden");
        }
        let elementAnswer1 = document.getElementById(`Aanswer_${button}`)
        let hidden2 = elementAnswer1.getAttribute("hidden");
        if (!hidden2) {
            changeQuest(button);
        }
    }
    else {
        for (let i = 0; i < element.length; i++) {
            element[i].setAttribute("hidden", "hidden");
            element2[i].setAttribute("hidden", "hidden");
        }
    }
    trig(hidden, button);
}