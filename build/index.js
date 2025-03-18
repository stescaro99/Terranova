var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
export function handleButtonClick(customName, customSurname, output) {
    return __awaiter(this, void 0, void 0, function* () {
        const name = customName.value.trim();
        const surname = customSurname.value.trim();
        if (name && surname) {
            output.textContent = `${name} ${surname}`;
            const response = yield fetch("http://10.0.2.15:5267/api/data", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ name, surname })
            });
        }
        else {
            output.textContent = "Inserisci sia il nome che il cognome.";
        }
    });
}
document.addEventListener("DOMContentLoaded", () => {
    const customName = document.getElementById("customName");
    const customSurname = document.getElementById("customSurname");
    const submitButton = document.getElementById("submitButton");
    const output = document.getElementById("output");
    if (customName && customSurname && submitButton && output) {
        submitButton.addEventListener("click", () => __awaiter(void 0, void 0, void 0, function* () {
            handleButtonClick(customName, customSurname, output);
        }));
    }
    else {
        console.error("❌ Elementi non trovati nel DOM!");
    }
});
//# sourceMappingURL=index.js.map