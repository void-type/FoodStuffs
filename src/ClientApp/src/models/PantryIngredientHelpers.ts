import type {
  GetMealSetResponsePantryIngredient,
  RecipeSearchResultItemIngredient,
} from '@/api/data-contracts';
import { isNil } from '@/models/FormatHelpers';

export function countIngredients(
  acc: GetMealSetResponsePantryIngredient[],
  curr: RecipeSearchResultItemIngredient
) {
  const { name, quantity } = curr;

  if (isNil(name)) {
    return acc;
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  let match = acc.find((x) => x.name === name!);

  if (!match) {
    match = { name, quantity: 0 };
    acc.push(match);
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  match.quantity! += quantity || 0;
  return acc;
}
export function addIngredient(
  ingredients: GetMealSetResponsePantryIngredient[],
  name: string,
  count = 1
) {
  let ingredient = ingredients.find((x) => x.name === name);

  if (!ingredient) {
    ingredient = { name, quantity: 0 };
    ingredients.push(ingredient);
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  ingredient.quantity! += count;
}
export function subtractIngredient(
  ingredients: GetMealSetResponsePantryIngredient[],
  name: string,
  count = 1
) {
  const ingredient = ingredients.find((x) => x.name === name);

  if (!ingredient) {
    return;
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  ingredient.quantity! -= count;

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  if (ingredient.quantity! < 1) {
    ingredients.splice(ingredients.indexOf(ingredient), 1);
  }
}
