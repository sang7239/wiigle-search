
function retrieveSuggestions(e) {
    let query = $('[name=search]').val().trim();
    e.which = e.which || e.keyCode;
    if (e.which == 13) {
        getSearchResults();
    } else {
        if (query) {
            $.ajax({
                type: "POST",
                url: "QuerySuggestionService.asmx/GetWordsWithPrefix",
                data: '{prefix:"' + query + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#suggestions").empty();
                    $("#results").empty();
                    let results = JSON.parse(msg.d);
                    results.forEach(function (result) {
                        $("<li>").addClass("list-group-item").html(result).appendTo("#suggestions");
                    });
                },

                error: function (msg) {
                    console.log(msg);
                }
            });
        } else {
            $("#suggestions").empty();
        } 
    }
}

function getSearchResults() {
    let query = $('[name=search]').val().trim();
    if (query) {
        $.ajax({
            type: "POST",
            url: "Admin.asmx/RetrieveSearchResults",
            data: '{searchWord:"' + query + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                $("#suggestions").empty();
                $('#results').empty();
                let results = JSON.parse(msg.d);
                results.forEach(function (result) {
                    $("#results").append(
                        $("<li>").addClass("list-group-item").append(
                            $("<a>").addClass("media-body").attr('href', result["Item2"]).attr('target', '_blank').prepend(
                                $("<h5>").addClass("mt-0 mb-1 font-weight-bold").html(result["Item1"])
                            )
                        )
                    );
                }); 
            },
            error: function (msg) {
                console.log(msg);
            }
        });
    } else {
        $("#results").empty();
    } 
}