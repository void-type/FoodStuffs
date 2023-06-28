import { defineStore } from 'pinia';
import type { IFailureIItemSet, IFailure } from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';

interface Message {
  text: string;
  fieldName: string | null;
  isError: boolean;
}

interface MessageStoreState {
  messages: Array<Message>;
}

export const useMessageStore = defineStore('messages', {
  state: (): MessageStoreState => ({
    messages: [],
  }),

  getters: {
    isFieldInError: (state) => (fieldName: string) =>
      state.messages.some((x) => x.fieldName === fieldName && x.isError === true),
  },

  actions: {
    setApiFailureMessages(response: HttpResponse<unknown, unknown>) {
      if (response === undefined || response === null) {
        this.setErrorMessage('Cannot connect to server.');
        return;
      }

      if (response.status === 401 || response.status === 403) {
        this.setErrorMessage('You are not authorized for this server endpoint.');
      } else if (response.status === 404) {
        this.setErrorMessage('Server responded with endpoint not found.');
      } else if (response.status >= 500) {
        const userMessage = response.error as IFailure;
        this.setErrorMessage(userMessage.message || '');
      } else {
        const failureSet = response.error as IFailureIItemSet;
        if (typeof failureSet !== 'undefined' && failureSet !== null) {
          this.setValidationErrorMessages(failureSet.items || []);
        } else {
          this.setErrorMessage(
            'Something went wrong. Try refreshing your browser or contact the administrator.'
          );
        }
      }
    },

    clearMessages() {
      this.messages.length = 0;
    },

    clearMessage(message: Message) {
      const index = this.messages.indexOf(message);
      this.messages.splice(index, 1);
    },

    setMessage(message: Message, autoClearSeconds: number | null = null) {
      this.messages.push(message);

      if (autoClearSeconds !== null && autoClearSeconds > 0) {
        setTimeout(() => {
          this.clearMessage(message);
        }, autoClearSeconds * 1000);
      }
    },

    setSuccessMessage(message: string) {
      this.setMessage({
        text: message,
        fieldName: null,
        isError: false,
      });
    },

    setErrorMessage(message: string) {
      this.messages.push({
        text: message,
        fieldName: null,
        isError: true,
      });
    },

    setValidationErrorMessages(failures: IFailure[]) {
      failures.forEach((x) => {
        this.messages.push({
          text: x.message || '',
          fieldName: x.uiHandle || null,
          isError: true,
        });
      });
    },
  },
});

export default useMessageStore;
