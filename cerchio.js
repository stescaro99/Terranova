"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var index_1 = require("./index");
document.addEventListener("DOMContentLoaded", function () {
    var canvas = document.getElementById("myCanvas");
    var ctx = canvas.getContext("2d");
    var output = document.getElementById("output");
    var customName = document.getElementById("customName");
    var customSurname = document.getElementById("customSurname");
    var circleX = 50;
    var circleY = 50;
    var radius = 40;
    if (ctx) {
        ctx.fillStyle = "black";
        ctx.beginPath();
        ctx.arc(50, 50, 40, 0, 2 * Math.PI);
        ctx.fill();
        ctx.fillStyle = "white"; // Colore del testo
        ctx.font = "20px Arial"; // Dimensione e font del testo
        ctx.textAlign = "center"; // Allinea il testo al centro
        ctx.textBaseline = "middle"; // Allinea il testo al centro
        ctx.fillText("invio", 50, 50); // Scrive il testo
    }
    canvas.addEventListener("click", function (event) {
        var rect = canvas.getBoundingClientRect();
        var mouseX = event.clientX - rect.left;
        var mouseY = event.clientY - rect.top;
        // Verifica se il click è dentro il cerchio
        var distance = Math.sqrt(Math.pow((mouseX - circleX), 2) + Math.pow((mouseY - circleY), 2));
        if (distance < radius) {
            (0, index_1.handleButtonClick)(customName, customSurname, output);
        }
    });
});
