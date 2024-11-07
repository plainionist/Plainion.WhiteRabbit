export interface ReportEntry {
  comment: string
  duration: number
}

export interface ReportVM {
  headline: string
  entries: ReportEntry[]
  total: string
}

export interface Activity {
  idx: number
  begin: Date | null
  end: Date | null
  comment: string
}
