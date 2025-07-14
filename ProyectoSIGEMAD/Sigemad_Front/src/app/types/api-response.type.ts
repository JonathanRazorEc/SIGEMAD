export type ApiResponse<T> = {
  count: number;
  page: number;
  Page?: number;
  pageSize: number;
  data: T;
  pageCount: number;
  pageIndex?: any
};
