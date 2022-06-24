export default interface IPaginatedRequest {
  isPagingEnabled?: boolean;
  page?: number;
  take?: number;
}
