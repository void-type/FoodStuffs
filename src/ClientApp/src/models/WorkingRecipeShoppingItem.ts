/* eslint-disable no-underscore-dangle */
import type {
  ListShoppingItemsResponse,
  SaveRecipeRequestShoppingItem,
} from '@/api/data-contracts';

export default class WorkingRecipeShoppingItem implements SaveRecipeRequestShoppingItem {
  // Used so Vue can track in editor.
  public uiKey = crypto.randomUUID();

  private _shoppingItemValue: ListShoppingItemsResponse | null = null;

  public set shoppingItemValue(value: ListShoppingItemsResponse | null) {
    this._shoppingItemValue = value;
    this.id = value?.id;
  }

  public get shoppingItemValue(): ListShoppingItemsResponse | null {
    return this._shoppingItemValue;
  }

  public id?: number;

  public quantity?: number = 1;

  public order?: number = Number.MAX_SAFE_INTEGER;
}
