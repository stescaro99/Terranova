import { handleButtonClick } from "./index";

document.addEventListener("DOMContentLoaded", () => {
    const canvas = document.getElementById("myCanvas") as HTMLCanvasElement;
    const ctx = canvas.getContext("2d");
    const output = document.getElementById("output") as HTMLElement;
    const customName = document.getElementById("customName") as HTMLInputElement;
    const customSurname = document.getElementById("customSurname") as HTMLInputElement;

    const circleX = 50;
    const circleY = 50;
    const radius = 40;

    if (ctx) {
        ctx.fillStyle = "black";
        ctx.beginPath();
        ctx.arc(50, 50, 40, 0, 2 * Math.PI);
        ctx.fill();
        
        ctx.fillStyle = "white";  // Colore del testo
        ctx.font = "20px Arial";  // Dimensione e font del testo
        ctx.textAlign = "center"; // Allinea il testo al centro
        ctx.textBaseline = "middle"; // Allinea il testo al centro

        ctx.fillText("invio", 50, 50); // Scrive il testo
    }

    canvas.addEventListener("click", (event) => {
        const rect = canvas.getBoundingClientRect();
        const mouseX = event.clientX - rect.left;
        const mouseY = event.clientY - rect.top;

        // Verifica se il click è dentro il cerchio
        const distance = Math.sqrt((mouseX - circleX) ** 2 + (mouseY - circleY) ** 2);
        if (distance < radius) {
            handleButtonClick(customName, customSurname, output);
            // output.textContent = "Cliccato!";
        }
    });
});
