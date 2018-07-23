import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import {environment} from '../../../environments/environment';
import { Vacation } from '../components/profile/my-vacations/vacation.model';

@Injectable()
export class VacationService {
    constructor (private http: HttpClient) { }

    getVacations(): Observable<Vacation[]> {
        let requestUrl = environment.baseUrl + '/vacations/';
        return this.http.get<Vacation[]>(`${requestUrl}`);
    }
}

   

