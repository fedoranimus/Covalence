import {inject, autoinject} from 'aurelia-framework';
import {Config} from 'aurelia-api';
import {Result, IResult} from '../infrastructure/result';
import { IUser } from '../infrastructure/user';

@autoinject
export class SearchService {
    constructor(private config: Config) {

    }

    public getResults(query: any): Promise<IResult[]> {
        return this.config.getEndpoint('api').post('search', query);
    }

    public getAllUsers(): Promise<IUser[]> {
        return this.config.getEndpoint('api').find('search/list');
    }
}