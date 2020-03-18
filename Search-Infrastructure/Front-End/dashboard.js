function startCrawl() {
    $.ajax({
        type: "POST",
        url: "Admin.asmx/StartCrawl",
        contentType: "text/html; charset=UTF-8",
        dataType: "text",
        success: function (msg) {
            $(".mb-0").empty();
            $(".mb-0").html(msg);
        },
        error: function (msg) {
            console.log(msg);
        }
    });
}
function pauseCrawl() {
    $.ajax({
        type: "POST",
        url: "Admin.asmx/PauseCrawl",
        contentType: "text/html; charset=UTF-8",
        dataType: "text",
        success: function (msg) {
            $(".mb-0").empty();
            $(".mb-0").html(msg);
        },
        error: function (msg) {
            console.log(msg);
        }
    });
}
