document.addEventListener("DOMContentLoaded", async function () {
    const appointmentsListContainer = document.querySelector(".appointments-list-container");

    appointmentsListContainer.addEventListener("click", async function (e) {
        const buttonMessage = e.target;

        if (!buttonMessage.classList.contains("btn-send-message")) return;

        const appoinementID = buttonMessage.querySelector("span");

        const response = await sendWhatsAppMessage(appoinementID.textContent);
        console.log(response);

        buttonMessage.innerHTML = "";

        if (response) {
            const icon = document.createElement("i");
            icon.classList.add("ph", "ph-chat-text", "appointment-icon");

            const message = document.createElement("p");
            message.textContent = "Mensagem Enviada";

            buttonMessage.appendChild(icon);
            buttonMessage.appendChild(message);
        }
    })

    const sendWhatsAppMessage = async function (appoinementID) {
        try {
            const apiKey = 5997205;
            const message = "nem sei oque mandar akakak"
            const phoneNumber = "+5518996630552";
            const url = `https://api.callmebot.com/whatsapp.php?phone=${phoneNumber}&text=${message}&apikey=${apiKey}`;
            fetch(url, { mode: 'no-cors' })
                .then(response => {
                    console.log(response.ok);
                    return response.ok
                
                })
                .catch(error => {
                    throw error;
                });

            

        } catch (error) {
            console.log(error)
            return false

        }
    }
});




   


