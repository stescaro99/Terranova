export async function handleButtonClick(customName: HTMLInputElement, customSurname: HTMLInputElement, output: HTMLElement) {
    const name = customName.value.trim();
    const surname = customSurname.value.trim();
    if (name && surname) {
        output.textContent = `${name} ${surname}`;
        const response = await fetch("http://10.0.2.15:5267/api/data", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ name, surname })
        });
    } else {
        output.textContent = "Inserisci sia il nome che il cognome.";
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const customName = document.getElementById("customName") as HTMLInputElement;
    const customSurname = document.getElementById("customSurname") as HTMLInputElement;
    const submitButton = document.getElementById("submitButton") as HTMLButtonElement;
    const output = document.getElementById("output") as HTMLElement;


    if (customName && customSurname && submitButton && output) {
        submitButton.addEventListener("click", async () => {
            handleButtonClick(customName, customSurname, output);
        });
    } else {
        console.error("❌ Elementi non trovati nel DOM!");
    }
});
