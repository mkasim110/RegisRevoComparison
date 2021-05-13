

$(document).ready(function () {

    //ShowPopup();
    $(function () {
        $('#txtUW').keyup(function () {
            if ($('#txtUW').val().length > 1) {
                $('#grdUWCount tr').hide();
                $('#grdUWCount tr:first').show();
                $('#grdUWCount tr td:containsNoCase(\'' + $('#txtUW').val() + '\')').parent().show();
            }
            else if ($('#txtUW').val().length == 0) {
                resetUWSearchValue();
            }

            if ($('#grdUWCount tr:visible').length == 1) {
                $('.norecords').remove();
                $('#grdUWCount').append('<tr class="norecords"><td colspan="6" class="Normal" style="text-align: center">No records were found</td></tr>');
            }
        });

        $('#txtUW').keyup(function (event) {
            if (event.keyCode == 27) {
                resetUWSearchValue();
            }
        });
    });

    function resetUWSearchValue() {
        $('#txtUW').val('');
        $('#grdUWCount tr').show();
        $('.norecords').remove();
        $('#txtUW').focus();
    }

    $.expr[":"].containsNoCase = function (el, i, m) {
        var search = m[3];
        if (!search) return false;
        return eval("/" + search + "/i").test($(el).text());
    };

    $(document).ready(function () {
        $('#txtProgramNumber').keyup(function () {
            if ($('#txtProgramNumber').val().length > 3) {
                $('#grdProgramCount tr').hide();
                $('#grdProgramCount tr:first').show();
                $('#grdProgramCount tr td:containsNoCase(\'' + $('#txtProgramNumber').val() + '\')').parent().show();
            }
            else if ($('#txtProgramNumber').val().length == 0) {
                resetSearchValue();
            }

            if ($('#grdProgramCount tr:visible').length == 1) {
                $('.norecords').remove();
                $('#grdProgramCount').append('<tr class="norecords"><td colspan="6" class="Normal" style="text-align: center">No records were found</td></tr>');
            }
        });

        $('#txtProgramNumber').keyup(function (event) {
            if (event.keyCode == 27) {
                resetSearchValue();
            }
        });
    });

    function resetSearchValue() {
        $('#txtProgramNumber').val('');
        $('#grdProgramCount tr').show();
        $('.norecords').remove();
        $('#txtProgramNumber').focus();
    }

    $(document).ready(function () {
        SetHeight();
    });

    function SetHeight() {

        $("#grdEntityCntStatus > tbody > tr").each(function (i, tr) {
            if ($("#grdEntityCntStatus > tbody > tr:nth-child(" + (i + 1) + ")").height() > $("#grdEntityCnt > tbody > tr:nth-child(" + (i + 1) + ")").height()) {
                $("#grdEntityCnt > tbody > tr:nth-child(" + (i + 1) + ")").height($("#grdEntityCntStatus > tbody > tr:nth-child(" + (i + 1) + ")").height());
            } else {
                $("#grdEntityCntStatus > tbody > tr:nth-child(" + (i + 1) + ")").height($("#grdEntityCnt > tbody > tr:nth-child(" + (i + 1) + ")").height());
            }

        });
    }
});



