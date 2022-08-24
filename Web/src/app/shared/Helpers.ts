export class Helpers {
  public static dataToFile(data: string, fileName: string): File {
    const arr = data.split(',');
    const mime = arr[0].match(/:(.*?);/)![1];

    const bstr = window.atob(arr[1]);
    let n = bstr.length;
    let u8arr = new Uint8Array(n);

    while (n--) {
      u8arr[n] = bstr.charCodeAt(n);
    }
    return new File([u8arr], fileName, { type: mime });
  }
}
