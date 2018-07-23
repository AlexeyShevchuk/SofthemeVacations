import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { Employee } from '../components/profile/my-profile/employee.model';
import { environment } from '../../../environments/environment';

@Injectable()
export class EditService {
    constructor (private http: HttpClient) { }

    updateEmployee(employee: Employee): Observable<Employee> {
        let requestUrl = environment.baseUrl + '/employees/f15e0f3d-ce3b-4b16-981b-0510fcf1fe45';
        const data = JSON.stringify(employee);
        return this.http.put(requestUrl, data).map(() => employee);
    }
}

