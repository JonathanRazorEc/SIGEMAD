import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class FireAuditoriaService {

    public http = inject(HttpClient);

    getFireDataById(id: Number):Observable<any>{
        let endpoint = `/Auditoria/incendio/${id}`;
        return this.http.get<any>(endpoint)
      }

}