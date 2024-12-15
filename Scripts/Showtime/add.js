function setupAutoCalculateTime(durationInMinutes) {
    const startingTimeInput = document.getElementById("startingTime");
    const endingTimeInput = document.getElementById("endingTime");

    startingTimeInput.addEventListener("input", function () {
        const startingTimeValue = this.value;

        if (startingTimeValue) {
            const [hour, minute] = startingTimeValue.split(":").map(Number);

            const totalMinutes = hour * 60 + minute + durationInMinutes;

            const endingHour = Math.floor(totalMinutes / 60) % 24; 
            const endingMinute = totalMinutes % 60;

            const formattedHour = endingHour.toString().padStart(2, "0");
            const formattedMinute = endingMinute.toString().padStart(2, "0");

            endingTimeInput.value = `${formattedHour}:${formattedMinute}`;
        }
    });
}


const mySubmit = () => {
    const form = document.querySelector("#myForm");

    const startingDateInput = document.querySelector("#startingTime");
    const endingDateInput = document.querySelector("#endingTime");

    // Lấy giá trị của Starting Date và Ending Date
    const startingDate = new Date(startingDateInput.value);
    const endingDate = new Date(endingDateInput.value);

    // Kiểm tra nếu Starting Date >= Ending Date
    if (!startingDateInput.value || !endingDateInput.value) {
        alert("Both Starting Time and Ending Time must be filled!");
    } else if (startingDate >= endingDate) {
        alert("Starting Time must be earlier than Ending Time!");
    } else if (!form.reportValidity()) {
    } else {
        $(document).ready(() => {
            token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                type: 'post',
                url: `/showtime/check`,
                data: {
                    __RequestVerificationToken: token,
                    cinema: $("input[name='cinema']").val(),
                    date: $("input[name='date']").val(),
                    startingTime: $("input[name='startingTime']").val(),
                    endingTime: $("input[name='endingTime']").val(),
                },
                success: function (response) {
                    if (response) {
                        alert("Starting Time or Ending Time is conflict with other showtime");
                    }
                    else {
                        form.submit();
                    }
                }
            })
        });
        
    }
};
