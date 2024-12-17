$(document).ready(function () {
    // Clear input fields on reset
    $('#btnRest').click(function () {
        $('form')[0].reset();
        $('.error-message').remove();  // Clear any existing error messages
    });

    // Handle form submission
    $('#btnSubmit').click(function (event) {
        event.preventDefault();  // Prevent the default form submission

        // Clear previous error messages
        $('.error-message').remove();

        var isValid = true;

        // Get values from form inputs
        var fullName = $('#fullName').val().trim();
        var phone = $('#phone').val().trim();
        var address = $('#address').val().trim();
        var email = $('#emailInfo').val().trim();
        var title = $('#title').val().trim();
        var content = $('#content').val().trim();

        // Validate Full Name
        if (fullName === '') {
            isValid = false;
            $('#fullName').after('<div class="error-message">Full name is required.</div>');
        }

        // Validate Phone Number
        if (phone === '') {
            isValid = false;
            $('#phone').after('<div class="error-message">Phone number is required.</div>');
        }

        // Validate Address
        if (address === '') {
            isValid = false;
            $('#address').after('<div class="error-message">Address is required.</div>');
        }

        // Validate Email
        if (email === '') {
            isValid = false;
            $('#emailInfo').after('<div class="error-message">Email is required.</div>');
        }

        // Validate Title
        if (title === '') {
            isValid = false;
            $('#title').after('<div class="error-message">Title is required.</div>');
        }

        // Validate Content
        if (content === '') {
            isValid = false;
            $('#content').after('<div class="error-message">Description is required.</div>');
        }

        // If form is valid, submit
        if (isValid) {
            alert('Form submitted successfully!');
            $('form')[0].reset();  // Reset the form
        } else {
            alert('Please fill in all required fields.');
        }
    });
});