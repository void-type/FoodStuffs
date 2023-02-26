const settingKeyEnableDarkMode = 'enableDarkMode';

export default class DarkModeHelpers {
  static getInitialDarkModeSetting() {
    const appPreference = localStorage.getItem(settingKeyEnableDarkMode);

    if (appPreference) {
      return appPreference === 'true';
    }

    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return true;
    }

    return false;
  }

  static setDarkMode(setting: boolean) {
    localStorage.setItem(settingKeyEnableDarkMode, setting.toString());

    const { body } = document;

    if (setting) {
      body.classList.add('bg-dark');
      body.classList.add('text-light');
      body.classList.remove('bg-light');
      body.classList.remove('text-dark');
    } else {
      body.classList.remove('bg-dark');
      body.classList.remove('text-light');
      body.classList.add('bg-light');
      body.classList.add('text-dark');
    }
  }
}
