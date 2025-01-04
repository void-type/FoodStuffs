import { defineStore } from 'pinia';
import type { AppVersion, DomainUser, WebClientInfo } from '@/api/data-contracts';
import type { ModalParameters } from '@/models/ModalParameters';
import DarkModeHelper from '@/models/DarkModeHelper';

interface AppStoreState {
  applicationName: string;
  fieldsInError: Array<string>;
  user: DomainUser;
  isInitialized: boolean;
  version: string;
  useDarkMode: boolean;
  modalIsActive: boolean;
  modalParameters: ModalParameters;
}

export const useAppStore = defineStore('app', {
  state: (): AppStoreState => ({
    applicationName: '',
    fieldsInError: [],
    user: {
      login: '',
      authorizedAs: [],
    },
    isInitialized: false,
    version: '',
    useDarkMode: false,
    modalIsActive: false,
    modalParameters: {
      title: '',
      description: '',
      okAction: undefined,
      cancelAction: undefined,
    },
  }),

  getters: {
    isFieldInError: (state) => (fieldName: string) =>
      state.fieldsInError
        .map((errorField) => errorField.toLowerCase())
        .indexOf(fieldName.toLowerCase()) > -1,

    isUserIsAuthorizedAs: (state) => (policy: string) =>
      (state.user.authorizedAs || []).includes(policy),
  },

  actions: {
    setApplicationInfo(data: WebClientInfo) {
      this.applicationName = data.applicationName || '';
      this.user = data.user || { login: '', authorizedAs: [] };
      this.isInitialized = true;
    },

    setVersionInfo(data: AppVersion) {
      let version = `v${data.version}`;

      if (data.isPublicRelease === false && data.gitCommitId) {
        const gitCommitId = data.gitCommitId.slice(0, 10);
        version += `-g${gitCommitId}`;
      }

      this.version = version;
    },

    setDarkMode(setting: boolean) {
      DarkModeHelper.setDarkMode(setting);
      this.useDarkMode = setting;
    },

    showModal(modalParameters: ModalParameters) {
      if (this.modalIsActive) {
        return;
      }

      this.modalIsActive = true;
      this.modalParameters = modalParameters;
    },

    hideModal() {
      this.modalIsActive = false;
      this.modalParameters = {
        title: '',
        description: '',
        okAction: undefined,
        cancelAction: undefined,
      };
    },
  },
});

export default useAppStore;
