$(document).ready(function () {

    /**
     * Displays a temporary message to the user, hides the main page content,
     * then restores it after a given duration and reloads the page.
     * @param {string} text - The message text to display.
     * @param {number} duration - The time in milliseconds before hiding the message and reloading.
     */
    function showMessage(text, duration) {
        $('.message').text(text).fadeIn();
        $('.page').fadeOut();
        setTimeout(() => {
            $('.message').fadeOut();
            $('.page').fadeIn();
            location.reload();
        }, duration);
    }

    /**
     * Collects and formats the data entered by the user into a color DTO (Data Transfer Object).
     * This object is used to send color data to the server in a structured way.
     * @returns {object} colorDto - The color data with Price, DisplayOrder, Name, and Description.
     */
    function createColorDto() {
        const colorDto = {
            Price: parseFloat($('#priceInput').val()),
            DisplayOrder: parseInt($('#orderInput').val(), 10),
            Name: $('#ColorName').val(),
            Description: $('#ColorInput').val()
        };
        return colorDto;
    }





    $(function () { // Short-hand for $(document).ready() — runs after the DOM is ready
        $("#sortable").sortable({ // Enable jQuery UI sortable functionality on the table body with id="sortable"
            items: "> tr", // Only allow <tr> elements (direct children) to be sortable
            axis: "y", // Restrict movement to the vertical axis
            cancel: ".btn, button, input, a", // Prevent drag action when clicking buttons, inputs, or links
            placeholder: "row-placeholder", // CSS class for the placeholder row during drag

            // Triggered when sorting stops (user finishes dragging)
            stop: function () {
                const items = [];

                // Loop through each row in the table after sorting
                $("#sortable tr").each(function (i) {
                    const id = $(this).data("id");
                    const order = i + 1;

                    // Update the order cell in the UI
                    $(this).find(".col-order").text(order);

                    // Push updated data into the array
                    items.push({ Id: id, DisplayOrder: order });
                });

                // Send updated order to the server
                $.ajax({
                    method: "POST",
                    url: "/index.aspx/SaveOrder",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ items })
                })
                    .done(function (res) {
                        const r = res.d;
                        if (!r.success) alert(r.message);
                    })
                    .fail(function (xhr) {
                        console.error(xhr);
                        alert("לא ניתן לשמור את הסדר");
                    });
            }
        });
    });


    // Listen for click events on elements with the class "btn-muted"
    // Using event delegation on document to handle dynamically generated elements
    $(document).on('click', '.btn-muted', function (e) {
        e.preventDefault();

        // Get the closest table row (<tr>) to the clicked button
        const $tr = $(this).closest('tr');

        // Extract data from table cells by their index:
        // eq(0) -> Name, eq(1) -> Price, eq(2) -> Color/Description, eq(3) -> Display Order
        const colorHax = $tr.find('td').eq(2).text().trim();
        const colorName = $tr.find('td').eq(0).text().trim();
        const price = $tr.find('td').eq(1).text().trim();
        const orderNum = $tr.find('td').eq(3).text().trim();

        // Fill the form inputs with the values from the selected row
        $('#priceInput').val(price);
        $('#orderInput').val(orderNum);
        $('#ColorName').val(colorName);
        $('#ColorInput').val(colorHax);

        // Enable the primary action button (e.g., Save/Update)
        $('.btn-primary').prop('disabled', false);
    });


    // Listen for click events on elements with the class "btn-danger"
    // Using event delegation on document to handle dynamically generated rows
    $(document).on('click', '.btn-danger', function (e) {
        e.preventDefault();

        // Get the color ID from the data-id attribute of the closest <tr>
        const colorId = parseInt($(this).closest('tr').data('id'), 10);

        // Send AJAX request to delete the color on the server
        $.ajax({
            method: 'POST',
            url: '/index.aspx/DeleteColor',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ id: colorId }),
        })
            .done(function (res) {
                const result = res.d;
                showMessage(result.message, 2000);
            })
            .fail(function (xhr) {
                console.error('Error', xhr.status, xhr.responseText);
                alert('קרתה שגיאת תקשורת מול השרת');
            });
    });



    // Listen for click events on elements with the class "btn-primary"
    // Using event delegation so it works for dynamically generated buttons
    $(document).on('click', '.btn-primary', function (e) {
        e.preventDefault();

        // Create a color DTO (Data Transfer Object) from form input fields
        // This function must be defined elsewhere in your code
        const UpdateColor = createColorDto();

        // Send AJAX request to update the color on the server
        $.ajax({
            method: 'POST',
            url: '/index.aspx/UpdateColor',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ color: UpdateColor }),
        })
            .done(function (res) {
                const result = res.d;
                showMessage(result.message, 2000);
            })
            .fail(function (xhr) {
                console.error('Error', xhr.status, xhr.responseText);
                alert('קרתה שגיאת תקשורת מול השרת');
            });
    });

    // Attach a click event listener to any element with the class `.btn-success`
    // This uses event delegation via `$(document).on()` so it also works for dynamically added elements
    $(document).on('click', '.btn-success', function (e) {
        e.preventDefault();

        // Validation: Check if the "In Stock" checkbox is selected
        // If it is not checked, show an alert and stop execution
        if (!$('#inStockInput').is(':checked')) {
            alert('לא ניתן להמשיך — יש לסמן שהפריט במלאי');
            return;
        }

        const addColor = createColorDto();

        // Perform an AJAX POST request to the server to add a new color
        $.ajax({
            method: 'POST',  
            url: '/index.aspx/AddNewColor',  
            contentType: 'application/json; charset=utf-8', 
            dataType: 'json', 
            data: JSON.stringify({ color: addColor }), 
        })
            .done(function (res) {
                // On success: extract the server response (`res.d`) and show a message
                const result = res.d;
                showMessage(result.message, 2000); 
            })
            .fail(function (xhr, textStatus, errorThrown) {
                // On failure: log detailed error information in the console
                console.error('AJAX FAIL', {
                    status: xhr.status,             
                    statusText: xhr.statusText,     
                    textStatus,                     
                    errorThrown,                    
                    responseText: xhr.responseText  
                });
                alert('אירעה שגיאת תקשורת עם השרת');
            });
    });



});

