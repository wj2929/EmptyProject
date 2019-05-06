var _PageCount = 0;
var _PageNum = 1;
var _Keyword="";

$(document).ready(function(){     
    GoPage(1);
});

function GoPage(num)
{
   _PageNum=num;
   $.ajax({
      type: "POST",
      url: "ResourceElectronList1.aspx/GetPage",
      data:'{inputkeyword:"' + _Keyword + '",inputpagenum:"' + _PageNum + '"}', 
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(msg) {
        $("#asyncZone").replaceWith('<tbody id="asyncZone">'+msg.d[0]+'</tbody>');
        _PageCount=msg.d[1];
         $("#pager").pager({ pagenumber: msg.d[2], pagecount: msg.d[1], buttonClickCallback: GoPage,allcount:msg.d[3] });
      }
    });
}
$("#Keyword").keydown(function(e){
    if(e.which == 13)
    {
        Search();
    }
});
function Search()
{
    _Keyword=$("#Keyword").val();
    _PageNum=1;
    GoPage(_PageNum);
}

function DelInfo(retVal,_refP1,_refP2) {
    yonLay(_refP1, _refP2, function (flag) {
        if (flag) {
            $.ajax({
                type: "POST",
                url: "ResourceElectronList1.aspx/DelInfo",
                data: '{DelGuid:"' + retVal + '"}', 
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    GoPage(_PageNum);
                }
            });
        }
    });
}

function DelInfos(_refP1,_refP2) {
    yonLay(_refP1, _refP2, function (flag) {
        if (flag) {
            var temp = "";
            var checkedvalues = [];
            $("#asyncZone input:checked").each(function () {
                checkedvalues.push(this.value);
            });
            $.ajax({
                type: "POST",
                url: "ResourceElectronList1.aspx/DelInfos",
                data: '{DelGuids:"' + checkedvalues.join(",") + '"}', 
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d == "ok") {
                        GoPage(_PageNum);
                    }
                    else {
                        alert(msg.d);
                    }
                }
            });
        }
    });
}