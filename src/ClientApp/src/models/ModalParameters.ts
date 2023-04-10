export interface ModalParameters {
  title: string;
  description: string;
  okAction?: (() => void) | undefined | null;
  cancelAction?: (() => void) | undefined | null;
}
