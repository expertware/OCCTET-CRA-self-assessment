export default {
    install(app) {
        app.config.globalProperties.$loadRecaptchaScript = () => {
            const script = document.createElement('script');
            script.src = `https://www.google.com/recaptcha/api.js?render=`+import.meta.env.VITE_GOOGLE_RECAPCHA_PUBLIC; 
            script.async = true;
            script.onload = () => { };
            document.body.appendChild(script);
        };
    }
};