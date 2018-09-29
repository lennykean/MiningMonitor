import { Component, OnInit } from '@angular/core';

import { Collector } from '../../models/Collector';
import { CollectorService } from '../collector.service';

@Component({
    templateUrl: './collectors.component.html',
    styleUrls: ['./collectors.component.scss']
})
export class CollectorsComponent implements OnInit {
    public collectors: Collector[];

    constructor(
        private collectorService: CollectorService) {
    }

    public async ngOnInit() {
        this.GetCollectors();
    }

    public async GetCollectors() {
        this.collectors = await this.collectorService.GetAll();
    }

    public async Approve(collector: Collector) {
        collector.approved = true;
        await this.collectorService.Update(collector);
    }

    public async Reject(collector: Collector) {
        collector.approved = false;
        await this.collectorService.Update(collector);
    }

    public async Delete(collector: Collector) {
        await this.collectorService.Delete(collector.id);
        this.GetCollectors();
    }

    public Status(collector: Collector) {
        if (collector.approved === true) {
            return 'Approved';
        }
        if (collector.approved === false) {
            return 'Rejected';
        }
        return 'Pending';
    }
}
