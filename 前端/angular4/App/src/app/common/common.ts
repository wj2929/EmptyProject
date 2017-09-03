/**
 * Created by 歌 on 2017/7/1.
 */
declare var $;
declare var bootbox;
declare var swal;
export function Error(Message: string) {
  bootbox.alert(Message);
}

export function SweetError(Title: string, Message: string) {
  swal(Title, Message, "error");
}


export function SweetSuccess(Title: string, Message: string) {
  swal(Title, Message, "success");
}


export function SweetConfirm(Title: string, Message: string, callback: (isConfirm: boolean) => void) {
  swal({
    title: Title,
    text: Message,
    type: "warning",
    showCancelButton: true,
    confirmButtonColor: "#DD6B55",
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    closeOnConfirm: true,
    closeOnCancel: true
  },
    function (isConfirm) {
      callback(isConfirm);
    });
}

export function SweetClose(){
  swal.close();
}


export function Success(Message: string) {
  bootbox.alert(Message);
}

export function ShowBlockUI() {
  $.blockUI();
}

export function HideBlockUI() {
  $.unblockUI();
}
