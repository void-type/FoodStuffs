export default interface IItemSet<T> {
  count?: number;
  items?: T[] | null;
  isPagingEnabled?: boolean;
  page?: number;
  take?: number;
  totalCount?: number;
}
