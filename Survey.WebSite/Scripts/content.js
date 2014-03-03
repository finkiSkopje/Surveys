document.body.className += " metro";
$(document).ready(function () {
    $("#TeacherId").on("change", function () {
        $(".assistant_name").html($(this).find(":selected").text());
    });
    $(".assistant_name").html($("#TeacherId").find(":selected").text());

    $(".metro-tile").click(function (ev) {
        ev.preventDefault();
        var el = $(this);
        var link = $(this).find('a');
        var clone = $("<div/>")
                        .addClass("expand-tile")
                        .css({
                            'position': 'absolute',
                            'left': el.offset().left,
                            'top': el.offset().top,
                            'width': el.width() + "px",
                            'height': el.height() + "px",
                            'z-index': '100'
                        })
                        .appendTo(document.body)
                        .animate({
                            left: $(window).scrollLeft(),
                            top: $(window).scrollTop(),
                            width: "100%",
                            height: "100%"
                        }, 500, function () {
                            setTimeout(function () {
                                window.location.href = link.attr('href');
                            }, 1000);
                        })
                        .append(
                            $('<img />')
                                .attr('src', '../Images/Update.png')
                                .css({
                                    'position': 'absolute',
                                    'left': ($(window).width() - 512) / 2,
                                    'top': ($(window).height() - 512) / 2
                                })
                        );
    });

    $(".tile-container .metro-tile").each(function () {
        $(this).css("opacity", "1");
    });
});