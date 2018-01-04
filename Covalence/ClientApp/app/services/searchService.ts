import { inject, autoinject } from 'aurelia-framework';
import { Config } from 'aurelia-api';
import { Result, IResult } from 'infrastructure/result';
import { IUser } from 'infrastructure/user';

@autoinject
export class SearchService {
    constructor(private config: Config) {

    }

    public getResults(query: any, page?: number): Promise<PagedList<IUser>> {
        return this.config.getEndpoint('api').post('search', { query: query, page: page } );
    }

    public getAllUsers(page?: number): Promise<PagedList<IUser>> {
        const url = `search/list/${page ? page : ''}`;
        return this.config.getEndpoint('api').post(url);
    }
}

export interface PagedList<T> {
    items: T[];
    hasNextPage: boolean;
    hasPreviousPage: boolean;
    pageNumber: number;
    totalPages: number;
}