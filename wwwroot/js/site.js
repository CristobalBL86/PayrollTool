//FOR LOADER

//$(function () {
//    $("#loaderbody").addClass('hide');

//    $(document).bind('ajaxStart', function () {
//        $("#loaderbody").removeClass('hide');
//    }).bind('ajaxStop', function () {
//        $("#loaderbody").addClass('hide');
//    });
//});

showPopUp = (url, title) => {
    $.ajax({
        type : "GET",
        url: url,
        success: function (res) {
            if (res.isValid) {
                $("#pop_addedit .modal-body").html(res.html);
                $("#pop_addedit .modal-title").html(title);
                $("#pop_addedit").modal('show');
            } else {
                $('#view-all').html(res.html)
                $('#pop_addedit .modal-body').html('');
                $('#pop_addedit .modal-title').html('');
                $('#pop_addedit').modal('hide');
            }
        }
    });
}

showPopUpAssistance = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#pop_assistance .modal-body").html(res);
            $("#pop_assistance .modal-title").html(title);
            $("#pop_assistance").modal('show');
        }
    });
}

showPopPayroll = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            if (res.isValid) {
                $("#pop_payroll .modal-body").html(res.html);
                $("#pop_payroll .modal-title").html(title);
                $("#pop_payroll").modal('show');
            } else {
                $('#view-payroll').html(res.html)
                $('#pop_payroll .modal-body').html('');
                $('#pop_payroll .modal-title').html('');
                $('#pop_payroll').modal('hide');
            }
        }
    });
}

popSubmit = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html);
                    $('#pop_addedit .modal-body').html('');
                    $('#pop_addedit .modal-title').html('');
                    $('#pop_addedit').modal('hide');

                    //$.notify('submitted successfully', { globalPosition: 'top center', className: 'success' });
                }
                else
                    $('#pop_addedit .modal-body').html(res.html);

            },
            error: function (err) {
                console.log(err)
            }
        })

        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

deleteRecord = form => {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all').html(res.html);

                    //$.notify('deleted successfully', { globalPosition: 'top center', className: 'success' });
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}

deleteAssistance = function() {
    if (confirm('Are you sure to delete this assistance?')) {
        return true;
    }
    return false;
}

getPayroll = (url) => {

    var calldata = {
        "stDate": $("#startDate").val(),
        "edDate": $("#endDate").val()
    };

    $.ajax({
        type: "GET",
        url: url,
        data: calldata,
        success: function (res) {
            if (res.isValid) {
                $("#view-payroll").html(res.html);
                $('#btnRelease').show();
            }
        }
    });
}

popSubmitPayroll = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-payroll').html(res.html);
                    $('#pop_payroll .modal-body').html('');
                    $('#pop_payroll .modal-title').html('');
                    $('#pop_payroll').modal('hide');
                    $('#btnRelease').show();

                    //$.notify('submitted successfully', { globalPosition: 'top center', className: 'success' });
                }
                else
                    $('#pop_payroll .modal-body').html(res.html);

            },
            error: function (err) {
                console.log(err)
            }
        })

        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function cleanScreen(){
    $('#btnRelease').hide();
    $('#view-payroll').html('')

    $.notify('Payroll file has been generated, exporting in process!', { globalPosition: 'top center', className: 'success' });
}