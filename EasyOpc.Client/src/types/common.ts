export type GetPageRequest = {
    page?: number | null;
    countInPage?: number | null;
}

export type GetPageResponse = {
    page: number;
    countInPage: number;
    totalCount: number;
}

export type PagingList<T> = {
    currentPage: number;
    countInPage: number;
    totalCount: number;
    items: T[]
}