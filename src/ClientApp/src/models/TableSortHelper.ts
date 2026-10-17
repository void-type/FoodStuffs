export interface TableSort {
  sortBy?: string | null;
  sortDescending?: boolean;
}

export default class TableSortHelper {
  // Clicking a column cycles ascending -> descending -> default sort.
  static next(current: TableSort, sortBy: string): { sortBy: string; sortDescending: boolean } {
    if (current.sortBy?.toLowerCase() !== sortBy.toLowerCase()) {
      return { sortBy, sortDescending: false };
    }

    if (!current.sortDescending) {
      return { sortBy, sortDescending: true };
    }

    return { sortBy: '', sortDescending: false };
  }
}
