import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Profile } from '../components/profile/my-profile/profile.model';
import { environment } from '../../../environments/environment';

@Injectable()
export class ProfileService {
    constructor (private http: HttpClient) { }

    getProfile(): Observable<Profile> {
        let requestUrl = environment.baseUrl + '/profile/current';
        return this.http.get<Profile>(`${requestUrl}`);
    }

    getProfileById(id: string): Observable<Profile> {
        let requestUrl = environment.baseUrl + '/profile/' + id;
        return this.http.get<Profile>(`${requestUrl}`);
    }

    getEmployees(): Observable<Profile[]> {
        let requestUrl = environment.baseUrl + '/employees';
        return this.http.get<Profile[]>(`${requestUrl}`);
    }
}