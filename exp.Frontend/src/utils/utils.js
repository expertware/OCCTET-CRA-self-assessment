import Swal from 'sweetalert2'
import moment from 'moment'
//#region SWAL
export default {
  ToastNotify(Type, Title, Timer) {
    if (!Timer) Timer = 2000
    Swal.fire({
      toast: true,
      position: 'top-right',
      showConfirmButton: false,
      timer: Timer,
      timerProgressBar: true,
      icon: Type,
      title: Title,
    })
  },
  ToastNotifySomethingWentWrong(Timer) {
    if (!Timer) Timer = 1500
    Swal.fire({
      toast: true,
      position: 'top-right',
      showConfirmButton: false,
      timer: Timer,
      timerProgressBar: true,
      icon: 'error',
      title: 'Something went wrong!',
    })
  },

  //#endregion

  //#region Images
  ShowDynamicImage(imagePath, defaultImg) {
    if (!imagePath) {
      const defaultimg = `images/${defaultImg}.png`;
      return new URL(`/src/assets/${defaultimg}`, import.meta.url).href;
    }
    return imagePath;
  },

  ShowDynamicImageSvg(imagePath, defaultImg) {
    if (!imagePath) {
      const defaultimg = `images/${defaultImg}.svg`;
      return new URL(`../assets/images/${defaultImg}.svg`, import.meta.url).href;
    }
    return imagePath;
  },
  

  //#endregion

//#region Date
    FormatDateDMY(date) {
    return moment(date).format("DD.MM.YYYY");
  },

  //#endregion


    TextLimiter(text, charactersLimit, defaultString) {
    if (text) {
      return text.length > charactersLimit
        ? text.substring(0, charactersLimit) + "..."
        : text;
    } else {
      return defaultString;
    }
  },
}
