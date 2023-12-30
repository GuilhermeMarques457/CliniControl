document.addEventListener("DOMContentLoaded", function () {

    let dentistID;
    const dentistIDInputHidden = document.querySelector("#DentistID");
    const appointmentDayInput = document.querySelector("#AppointmentDay");
    const startTimeInput = document.querySelector("#StartTimeInput");
    const endTimeInput = document.querySelector("#EndTimeInput");

    startTimeInput.disabled = true;
    endTimeInput.disabled = true;

    const createHourOption = function (hour) {
        const option = document.createElement("option");
        option.value = hour;
        option.className = "available-hour-option";
        option.textContent = hour;

        return option;
    }

    const timeToMinutes = function (time) {
        const [hours, minutes] = time.split(':').map(Number);
        return hours * 60 + minutes;
    }

    const reset = () => {
        startTimeInput.value = '';
        endTimeInput.value = ''

        startTimeInput.innerHTML = '';
        endTimeInput.innerHTML = '';

        startTimeInput.disabled = true;
        endTimeInput.disabled = true;

        appointmentDayInput.value = '';
        appointmentDayInput.disabled = false;
    }

    if (!dentistIDInputHidden) {
        appointmentDayInput.disabled = true;
        const dentistIdInput = document.querySelector("#DentistIdInput");

        dentistIdInput.addEventListener("change", function () {
            dentistID = dentistIdInput.value;

            reset();
        })
    }

    appointmentDayInput.addEventListener("change", async function () {
        const selectedDate = new Date(appointmentDayInput.value);

        if (dentistIDInputHidden) {
            dentistID = dentistIDInputHidden.value;
        }
        

        var ontem = new Date();
        ontem.setDate(ontem.getDate() - 1);
        ontem.setHours(0);

        if (selectedDate >= ontem) {
            startTimeInput.innerHTML = "";

            const response = await fetch(`/Appointment/GetAvailableHours/${appointmentDayInput.value}/${dentistID}`);
            const data = await response.json();

            const availableTimes = data.availableTimes;
           
            availableTimes.forEach(time => {
                const option = createHourOption(time);

                startTimeInput.appendChild(option);
            });

            startTimeInput.disabled = false;
        }
    });

   
    startTimeInput.addEventListener("click", async function () {
        endTimeInput.innerHTML = "";

        // TENHO QUE COLOCAR MAIS 30 MINS NESSE SELECTED VALUE PARA EU PODER SELECIONAR UM HORARIO QUE FAÇA SENTIDO TIPO MESMO TENDO CONSULTA COMEÇANDO AS 8 EU POSSO MARCAR DAS 7 E MEIA AS 8 xD
        const selectedValue = startTimeInput.value;
        const availableHours = document.querySelectorAll(".available-hour-option");
        let optionsToKeep = [];

        for (let i = 0; i < availableHours.length; i++) {
            const time = availableHours[i].value;

            if (selectedValue <= time) {
                optionsToKeep.push(time)
            }
        }

        for (let i = 1; i < optionsToKeep.length; i++) {
            const currentTime = optionsToKeep[i];
            let pastTime = optionsToKeep[i - 1];

            const currentTimeMinutes = timeToMinutes(currentTime);
            const pastTimeMinutes = timeToMinutes(pastTime);

            if (currentTimeMinutes > pastTimeMinutes + 30) {
                break;
            }

            const option = createHourOption(currentTime);
            endTimeInput.appendChild(option);
        }


        endTimeInput.disabled = false;
    })
});
