export type Page<T> = {
    pageNumber: number;
    countInPage: number;
    totalCount: number;
    items: T[]
}